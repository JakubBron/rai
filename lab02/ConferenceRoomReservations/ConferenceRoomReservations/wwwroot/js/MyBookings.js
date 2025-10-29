async function cancelReservation(roomId, startTime) {
    if (!confirm("Czy na pewno chcesz potwierdzić operację?")) return;

    try {
        const response = await fetch(`/Booking/Cancel?beginTime=${encodeURIComponent(startTime)}&id=${roomId}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json" // optional for DELETE
            }
        });

        const result = await response.json();

        if (response.ok) {
            alert(result.message || "Reservation cancelled!");
            location.reload(); // refresh page or table
        } else {
            alert(result.error || "Failed to cancel reservation.");
        }
    } catch (err) {
        console.error(err);
        alert("Network error while cancelling.");
    }
}
