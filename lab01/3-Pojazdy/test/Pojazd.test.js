"use strict";

const { expect } = require("chai");
const Pojazd = require("../prod/Pojazd.js");

describe("Zad. 2. Testy klasy Pojazd (closures, zmienne prywatne)", () => {
  let pojazd;

  beforeEach(() => {
    pojazd = Pojazd("A1", 200);
  });

  it("powinien poprawnie ustawić prędkość po starcie", () => {
    const wynik = pojazd.start(100);
    expect(wynik).to.equal(100);
    expect(pojazd.status()).to.include("100/200");
  });

  it("nie powinien przekraczać maksymalnej prędkości", () => {
    pojazd.start(300);
    expect(pojazd.status()).to.include("200/200");
  });

  it("powinien zatrzymać pojazd po stop()", () => {
    pojazd.start(150);
    pojazd.stop();
    expect(pojazd.status()).to.include("0/200");
  });

  it("nie powinien pozwalać na dostęp do prywatnych zmiennych", () => {
    expect(pojazd.predkosc).to.be.undefined;
    expect(pojazd.id).to.be.undefined;
    expect(pojazd.max_predkosc).to.be.undefined;
  });
});
