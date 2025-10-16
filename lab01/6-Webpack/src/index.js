import Book from "./book.js";
import Library from "./library.js";

const lib = new Library();
lib.addBook(new Book("Solaris", "S. Lem", 2025, ["sci-fi", "filozofia"]));
lib.addBook(new Book("Fiasko", "Lem S.", 1990, ["sci-fi"]));

const output = document.getElementById("output");
output.textContent = "Books in library: " + lib.books.length;

const output2 = document.getElementById("output2");
if (output2) {
    // create table
    const table = document.createElement("table");
    
    // header
    const thead = document.createElement("thead");
    thead.innerHTML = "<tr><th>Title</th><th>Author</th></tr>";
    table.appendChild(thead);

    // body
    const tbody = document.createElement("tbody");
    lib.books.forEach(b => {
        const tr = document.createElement("tr");
        const tdAuthor = document.createElement("td");
        tdAuthor.textContent = b.author || "";
        tdAuthor.style.border = "1px solid black";

        const tdTitle = document.createElement("td");
        tdTitle.textContent = b.title || "";
        tdTitle.style.border = "1px solid black";

        tr.appendChild(tdAuthor);
        tr.appendChild(tdTitle);
        tbody.appendChild(tr);
    });
    table.appendChild(tbody);

    // render
    output2.innerHTML = "";
    output2.appendChild(table);
}


// Hot Module Reload example
if (import.meta.webpackHot) {
  import.meta.webpackHot.accept();
}
