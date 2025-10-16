const Book = require('./book.js');

"use strict";


"use strict";

class Library {
    constructor() {
        this.books = [];
        this.rents = new Map();
    }

    addBook(book) {
        this.books.push(book);
    }

    getBooks() {
        return this.books;
    }

    rent(bookTitle, person) {
        const matchingBooks = this.books.filter(k => k.title === bookTitle);

        if (matchingBooks.length === 0) {
            throw new Error(`Nie znaleziono książki o tytule "${bookTitle}"`);
        }

        // wybierz pierwszy dostępny egzemplarz
        const availableBook = matchingBooks.find(k => k.isAvailable());

        if (!availableBook) {
            throw new Error(`Wszystkie egzemplarze tej książki są już wypożyczone`);
        }

        // oznacz jako wypożyczoną i zapisz w mapie
        availableBook.markAsUnavailable();
        this.rents.set(availableBook, person);

        return availableBook;
    }

    returnBook(bookTitle, person) {
        const bookEntry = [...this.rents.entries()]
            .find(([book, borrower]) => book.title === bookTitle && borrower === person);

        if (!bookEntry) {
            throw new Error(`Nie znaleziono wypożyczonej książki "${bookTitle}" przez ${person}`);
        }

        const [book] = bookEntry;
        book.markAsAvailable();
        //this.rents.delete(book);

        return book;
    }

    whoRented(title) {
        return [...this.rents.entries()].filter(([book]) => book.title === title).map(([, borrower]) => borrower);
    }

    searchByKeywords(keywords) {
        const keys = Array.isArray(keywords) ? keywords : [keywords];
        const normalized = new Set(keys.map(s => s.toLowerCase()));

        return this.books.filter(book =>
            book.keywords.some(k => normalized.has(k.toLowerCase()))
        );
    }
}

module.exports = Library;
