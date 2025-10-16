// we DONT wanna append new methods in code anywhere else but in this place
// we DISALLOW operations on class fields without proper functions
"use strict";

class Book {
    constructor(title, author, yearOfPublication, keywords = []) {
        this.author = author;
        this.title = title;
        this.yearOfPublication = yearOfPublication;
        this.keywords = keywords;
        this.available = true;
    }

    markAsUnavailable() {
        this.available = false;
    }

    markAsAvailable() {
        this.available = true;
    }

    isAvailable() {
        return this.available;
    }

    getTitle() {
        return this.title;
    }

    getAuthor() {
        return this.author;
    }

    getYearOfPublication() {
        return this.yearOfPublication;
    }

    getKeywords() {
        return this.keywords;
    }
}

module.exports = Book;