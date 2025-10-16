const chai = require('chai');

const Library = require('../prod/library');
const Book = require('../prod/book');

const expect = chai.expect;

describe('Library', () => {
    let library;
    let book1, book2, book3;
    let librarySize = 0;

    beforeEach(() => {
        library = new Library();
        book1 = new Book('Title1', "Author1", 2002, ['fiction', 'adventure']);
        book2 = new Book('Title2', "Author2", 1997, ['history']);
        book3 = new Book('Title3', "Author3", 2010);
        book4 = new Book('Title3', "Author3", 2012);
        library.addBook(book1);
        library.addBook(book2);
        library.addBook(book3);
        library.addBook(book4);
        librarySize = 4;

    });

    it('should add books and getBooks returns all books', () => {
        expect(library.getBooks()).to.have.lengthOf(librarySize);
        expect(library.getBooks()).to.include.members([book1, book2, book3, book4]);
    });

    it('should rent an available book by title', () => {
        const rented = library.rent('Title1', 'Alice Smith');
        expect(rented.title).to.equal('Title1');
        expect(rented.isAvailable()).to.be.false;
        expect(library.whoRented('Title1')).to.include('Alice Smith');
    });

    it('should throw error if book title not found when renting', () => {
        expect(() => library.rent('Unknown', 'Bob')).to.throw('Nie znaleziono książki o tytule "Unknown"');
    });

    it('should throw error if all copies are rented', () => {
        library.rent('Title3', 'Alice');
        library.rent('Title3', 'Bob');
        expect(() => library.rent('Title3', 'Charlie')).to.throw('Wszystkie egzemplarze tej książki są już wypożyczone');
    });

    it('should return a rented book', () => {
        const rented = library.rent('Title2', 'Dave');
        expect(rented.isAvailable()).to.be.false;

        const returned = library.returnBook('Title2', 'Dave');
        expect(returned.isAvailable()).to.be.true;
        expect(library.whoRented('Title2')).to.include('Dave');
    });

    it('should throw error when returning a book not rented by person', () => {
        library.rent('Title2', 'Eve');
        expect(() => library.returnBook('Title2', 'Frank')).to.throw('Nie znaleziono wypożyczonej książki "Title2" przez Frank');
    });

    it('should list all people who rented a book by title', () => {
        library.rent('Title3', 'Alice');
        library.rent('Title3', 'Bob');
        expect(library.whoRented('Title3')).to.have.members(['Alice', 'Bob']);
    });

    it('should search books by keywords (case insensitive)', () => {
        const results = library.searchByKeywords(['Fiction', 'history']);
        expect(results).to.include.members([book1, book2]);
    });

    it('should search books by single keyword string', () => {
        const results = library.searchByKeywords('adventure');
        expect(results).to.include(book1);
        expect(results).to.not.include(book2);
    });

    it('should return empty array if no books match keywords', () => {
        const results = library.searchByKeywords('nonexistent');
        expect(results).to.be.an('array').that.is.empty;
    });
});