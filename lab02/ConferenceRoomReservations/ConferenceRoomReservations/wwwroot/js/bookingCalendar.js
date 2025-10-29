"use strict";

// globals
let minDuration = 1;
let conferenceRooms = [];
let takenRooms = [];

// helpers
async function fetchConfigFromApi() {
    const response = await fetch("/Booking/GetConfig");
    const data = await response.json();
    return parseInt(data.split(":")[1], 10);
}

async function fetchConferenceRooms() {
    const response = await fetch("/Booking/GetConferenceRooms");
    const data = await response.json();
    return data;
}

// TODO: fetch usage of conference rooms!
async function fetchTakenRooms() {
    const response = await fetch("/Booking/GetForDay");
    const data = await response.json();
    return data;
}

function getStatus(hour, minute, conferenceRoom) {
    const slotStart = new Date();
    slotStart.setHours(hour, minute, 0, 0);

    const slotEnd = new Date(slotStart.getTime() + minDuration * 60 * 1000);

    for (const res of takenRooms) {
        if (res.roomId === conferenceRoom.id) {
            const resStart = new Date(res.startTime);
            const resEnd = new Date(res.endTime);

            const overlap =
                slotStart < resEnd && slotEnd > resStart;

            if (overlap) {
                
                return "taken";
            }
        }
    }
    return "free";
}

async function sendReservationRequest(beginTime, id) {
    const formData = new URLSearchParams();
    formData.append("beginTime", beginTime);
    formData.append("id", id);


    const response = await fetch("/Booking/Create", {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: formData
    });

    if (response.ok) {
        const result = await response.json();
        alert(result["message"]);
        location.reload();
        
    } else {
        const error = await response.json();
        alert(error["error"]);
        location.reload();
    }
}


// main function
async function buildTable() {
    // wait for async data
    minDuration = await fetchConfigFromApi();
    conferenceRooms = await fetchConferenceRooms();
    takenRooms = await fetchTakenRooms();
    

    const divCalendar = document.getElementById("calendar");
    divCalendar.innerHTML = "";
    const reserveButton = document.getElementById("buttonTemplate").innerHTML;

    const calendarTable = document.createElement("table");
    calendarTable.className = "dataTable";

    // make table header
    const tableHeader = document.createElement("thead");
    const headerRow = document.createElement("tr");

    // first empty cell
    headerRow.appendChild(document.createElement("th"));
    for (let room of conferenceRooms) {
        let th = document.createElement("th");
        th.innerText = room.name;
        headerRow.appendChild(th);
    }
    tableHeader.appendChild(headerRow);

    // make table body
    const tableBody = document.createElement("tbody");
    for (let hour = 6; hour < 22; hour++) {
        for (let minute = 0; minute < 60; minute += minDuration) {
            const row = document.createElement("tr");
            const timeCell = document.createElement("td");

            let reservationTime = `${String(hour).padStart(2, "0")}:${String(minute).padStart(2, "0")}` 
            timeCell.innerText = reservationTime;
            row.appendChild(timeCell);

            for (let i = 0; i < conferenceRooms.length; i++) {
                let cell = document.createElement("td");
                cell.className = "centered";

                let conferenceRoomStatus = getStatus(hour, minute, conferenceRooms[i]);
                if (conferenceRoomStatus === "free") {
                    cell.innerHTML = reserveButton;
                    
                    const btn = cell.querySelector(".reserve-btn");
                    btn.addEventListener("click", () => {
                        return sendReservationRequest(reservationTime, conferenceRooms[i].id);
                    });
                }
                else {
                    cell.innerHTML = "";
                    cell.innerText = "❌";
                }                
                row.appendChild(cell);
            }
            tableBody.appendChild(row);
        }
    }

    // assemble table
    calendarTable.appendChild(tableHeader);
    calendarTable.appendChild(tableBody);
    divCalendar.appendChild(calendarTable);
}

// run when DOM is ready
document.addEventListener("DOMContentLoaded", buildTable);
