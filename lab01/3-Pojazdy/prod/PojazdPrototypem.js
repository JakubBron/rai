"use strict";

// constructor
function PojazdPrototypem(id, max_predkosc) {
  this.id = id;
  this.max_predkosc = max_predkosc;
  this.predkosc = 0;
}

// 
PojazdPrototypem.prototype.start = function (nowaPredkosc) {
  if (nowaPredkosc > this.max_predkosc) {
    this.predkosc = this.max_predkosc;
  }
  else {
    this.predkosc = nowaPredkosc;
  }
  return this.predkosc;
}

PojazdPrototypem.prototype.stop = function () {
  this.predkosc = 0;
}

PojazdPrototypem.prototype.status = function () {
  return `Pojazd ${this.id}: prędkość = ${this.predkosc}/${this.max_predkosc}`;
}

module.exports = PojazdPrototypem;