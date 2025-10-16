"use strict";

// constructor AND all of class methods are inside THIS and ONLY THIS object, that means are private. Not visible outside of Pojazd. 
// Valid use: Pojazd.method
// Invalid use: method()
function Pojazd(id, max_predkosc) {
  let predkosc = 0;

  return {
    start: function (nowaPredkosc) {
      if (nowaPredkosc > max_predkosc) {
        predkosc = max_predkosc;
      } else {
        predkosc = nowaPredkosc;
      }
      return predkosc;
    },

    stop: function () {
      predkosc = 0;
      return predkosc;
    },

    status: function () {
      return `Pojazd ${id}: prędkość = ${predkosc}/${max_predkosc}`;
    }
  };
}

module.exports = Pojazd;