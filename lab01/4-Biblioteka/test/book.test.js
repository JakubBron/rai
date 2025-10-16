"use strict";

const { expect } = require("chai");
const Book = require("../prod/book.js");

describe("Zad. 4. Testy klasy Book", () => {
    let book;
    let title = "The Great Gatsby";
    let author = "F. Scott Fitzgerald";
    let year = 1925;
    let keywords = ["classic", "novel"];
    const bookNoKeywords = new Book(title, author, year);


    beforeEach(() => {
        book = new Book(title, author, year, keywords);
    });

    it("powinien poprawnie zwracać tytuł", () => {
        expect(book.getTitle()).to.equal(title);
    });

    it("powinien poprawnie zwracać autora", () => {
        expect(book.getAuthor()).to.equal(author);
    });

    it("powinien poprawnie zwracać rok publikacji", () => {
        expect(book.getYearOfPublication()).to.equal(year);
    });

    it("powinien poprawnie zwracać słowa kluczowe", () => {
        expect(book.getKeywords()).to.eql(keywords);
    });

    it("powinien poprawnie tworzyć książkę bez słów kluczowych", () => {
        expect(bookNoKeywords.getKeywords()).to.eql([]);
    });
    
    it("powinien być dostępny do wypożyczenia na początku", () => {
        expect(book.isAvailable()).to.be.true;
    });

    it("powinien oznaczyć jako wypożyczoną", () => {
        book.markAsUnavailable();
        const wynik = book.isAvailable();
        expect(wynik).to.be.false;
    })

    it("powinien oznaczyć jako możliwą do wypożyczenia", () => {
        book.markAsAvailable();
        const wynik = book.isAvailable();
        expect(wynik).to.be.true;
    });


});