"use strict";

// zad. II (sandbox)
const Pojazd = require("./prod/Pojazd.js");
let pojazd = Pojazd("pojazd 1", 200);
pojazd.start(100);

console.log("Wywołania dla obiektu 'pojazd'");
//console.log(pojazd.id); // should undefined cause private
console.log(pojazd.status());   // used as function
console.log(pojazd.stop());   // used as function
console.log(pojazd.status());   // used as function
// NOTE: it is impossible to access object variables

console.log("--------------------");

const PojazdPrototypem = require("./prod/PojazdPrototypem.js");
let pojazdP = new PojazdPrototypem("pojazdP 1", 200);
pojazdP.start(100);


console.log("Wywołania dla obiektu 'pojazd'");
//console.log(pojazd.id); // should undefined cause private
console.log(pojazdP.status());   // used as function
console.log(pojazdP.stop());   // used as function
console.log(pojazdP.status());   // used as function
pojazdP.predkosc = 50; // possible to change public fields
console.log(pojazdP.status() + " A nie użyłem metody!");   // used as function
PojazdPrototypem.prototype.nowaMetoda =  function() { return "A teraz sobie dopiszę do prototypu metodę z dupy"; } // possible to add new method to prototype
console.log(pojazdP.nowaMetoda()); // possible to use new method
