"use strict";

const { expect } = require("chai");
const PojazdPrototypem = require("../prod/PojazdPrototypem.js");

describe("Zad. 2. Testy klasy PojazdPrototypem (prototypes)", () => {
  let p;

  beforeEach(() => {
    p = new PojazdPrototypem("A1", 200);
  });

  it("powinien poprawnie ustawić prędkość po starcie", () => {
    expect(p.start(100)).to.equal(100);
    expect(p.status()).to.include("100/200");
  });

  it("nie powinien przekraczać maksymalnej prędkości", () => {
    p.start(300);
    expect(p.status()).to.include("200/200");
  });

  it("powinien zatrzymać pojazd", () => {
    p.start(150);
    p.stop();
    expect(p.predkosc).to.equal(0);
  });

  it("pola obiektu są publiczne i można je zmienić", () => {
    p.id = "B2";
    p.max_predkosc = 180;
    p.predkosc = 50;

    expect(p.id).to.equal("B2");
    expect(p.max_predkosc).to.equal(180);
    expect(p.predkosc).to.equal(50);
  });

  it("powinien mieć dostęp do właściwości prototype, constructor, __proto__", () => {
    expect(PojazdPrototypem.prototype).to.be.an("object");
    expect(p.constructor).to.equal(PojazdPrototypem);
    expect(Object.getPrototypeOf(p)).to.equal(PojazdPrototypem.prototype);
  });

  it("powinien korzystać z funkcji w prototypie (te same metody współdzielone)", () => {
    const p2 = new PojazdPrototypem("A2", 150);
    expect(p.start).to.equal(p2.start); // te same metody z prototypu
  });

  it("nowa funkcja dodana do prototypu po utworzeniu obiektu powinna być dostępna", () => {
    // dodajemy nową funkcję do prototypu
    PojazdPrototypem.prototype.nowaMetoda = function () {
      return `Jestem nowoutworzoną metodą obiektu ${this.id}`;
    };

    expect(p.nowaMetoda()).to.equal("Jestem nowoutworzoną metodą obiektu A1"); // obiekt utworzony wcześniej też ma nową metodę!
  });
});
