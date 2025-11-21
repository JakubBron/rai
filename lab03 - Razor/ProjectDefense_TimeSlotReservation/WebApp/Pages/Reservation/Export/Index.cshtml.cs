using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using DataModelsLib.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;
using DataModelsLib.CustomTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebApp.Data.Migrations;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using Document = QuestPDF.Fluent.Document;
using Microsoft.AspNetCore.Identity;
using WebApp.ModelsInternal;

namespace WebApp.Pages.Reservation.Export
{
    public class ExportInputModel: IValidatableObject
    {
        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Format")]
        public string Format { get; set; } = "txt";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult(ValidationResultsMessages.StartTimeAfterEndTime, new[] { nameof(EndDate) });
            }
        }
    }

    [Authorize(Roles = RoleNames.Prowadz¹cy)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty] public ExportInputModel Input { get; set; } = new();

        public List<Room> Rooms { get; set; } = new();

        public void OnGet()
        {
            Rooms = _context.Rooms.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Rooms = _context.Rooms.ToList();
                return Page();
            }

            var reservations = await _context.Reservations
                .Where(r => r.StarTime >= Input.StartDate && r.EndTime <= Input.EndDate)
                .OrderBy(r => r.StarTime)
                .ToListAsync();

            foreach (var r in reservations)
            {
                var student = await _userManager.FindByIdAsync(r.StudentId);
                r.StudentId = student != null ? student.Email : "Free";
            }

            var roomName = _context.Rooms.Where(r => r.Id == Input.RoomId)
                .Select(r => r.RoomName.Replace(" ", "_"))
                .FirstOrDefault() ?? "UnknownRoom";

            string fileName = $"Reservations_{Input.StartDate:yyyyMMdd}_{Input.EndDate:yyyyMMdd}_{roomName}";

            switch (Input.Format.ToLower())
            {
                case "txt":
                    var txtContent = _exportToTxt(reservations);
                    return File(System.Text.Encoding.UTF8.GetBytes(txtContent), "text/plain", $"{fileName}.txt");

                case "xlsx":
                    var xmlContent = _exportToXML(reservations);
                    return File(xmlContent.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{fileName}.xlsx");

                case "pdf":
                    var pdfContent = _exportToPDF(reservations);
                    return File(pdfContent.ToArray(), "application/pdf", $"{fileName}.pdf");

                default:
                    ModelState.AddModelError(string.Empty, "Unsupported format.");
                    Rooms = _context.Rooms.ToList();
                    return Page();
            }
        }

        private string _exportToTxt(IEnumerable<ReservationsModel> reservations)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Start Time\tEnd Time\tStudent");
            foreach (var r in reservations)
            {
                sb.AppendLine($"{r.StarTime:yyyy-MM-dd HH:mm}\t{r.EndTime:HH:mm}\t{r.StudentId ?? "Free"}");
            }
            return sb.ToString();
        }

        private MemoryStream _exportToXML(IEnumerable<ReservationsModel> reservations)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Reservations");
            ws.Cell(1, 1).Value = "Start Time";
            ws.Cell(1, 2).Value = "End Time";
            ws.Cell(1, 3).Value = "Student";
            int row = 2;
            foreach (var r in reservations)
            {
                ws.Cell(row, 1).Value = r.StarTime;
                ws.Cell(row, 2).Value = r.EndTime;
                ws.Cell(row, 3).Value = r.StudentId ?? "Free";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        private MemoryStream _exportToPDF(IEnumerable<ReservationsModel> reservations)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var pdfStream = new MemoryStream();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.ConstantColumn(30);
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Start");
                            header.Cell().Text(" ");
                            header.Cell().Text("End");
                            header.Cell().Text("Student");
                        });

                        foreach (var r in reservations)
                        {
                            table.Cell().Text(r.StarTime.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().Text("->");
                            table.Cell().Text(r.EndTime.ToString("HH:mm"));
                            table.Cell().Text(r.StudentId ?? "Free");
                        }
                    });
                });
            }).GeneratePdf(pdfStream);
            pdfStream.Position = 0;
            return pdfStream;
        }
    }
}
