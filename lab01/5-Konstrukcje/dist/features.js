"use strict";

function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(a, n) { if (!(a instanceof n)) throw new TypeError("Cannot call a class as a function"); }
function _defineProperties(e, r) { for (var t = 0; t < r.length; t++) { var o = r[t]; o.enumerable = o.enumerable || !1, o.configurable = !0, "value" in o && (o.writable = !0), Object.defineProperty(e, _toPropertyKey(o.key), o); } }
function _createClass(e, r, t) { return r && _defineProperties(e.prototype, r), t && _defineProperties(e, t), Object.defineProperty(e, "prototype", { writable: !1 }), e; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
function _classPrivateMethodInitSpec(e, a) { _checkPrivateRedeclaration(e, a), a.add(e); }
function _classPrivateFieldInitSpec(e, t, a) { _checkPrivateRedeclaration(e, t), t.set(e, a); }
function _checkPrivateRedeclaration(e, t) { if (t.has(e)) throw new TypeError("Cannot initialize the same private elements twice on an object"); }
function _classPrivateFieldGet(s, a) { return s.get(_assertClassBrand(s, a)); }
function _classPrivateFieldSet(s, a, r) { return s.set(_assertClassBrand(s, a), r), r; }
function _assertClassBrand(e, t, n) { if ("function" == typeof e ? e === t : e.has(t)) return arguments.length < 3 ? t : n; throw new TypeError("Private element is not present on this object"); }
var _age = /*#__PURE__*/new WeakMap();
var _Person_brand = /*#__PURE__*/new WeakSet();
var Person = /*#__PURE__*/function () {
  // Private field

  function Person(name, age) {
    _classCallCheck(this, Person);
    // Private method
    _classPrivateMethodInitSpec(this, _Person_brand);
    _classPrivateFieldInitSpec(this, _age, void 0);
    this.name = name;
    _classPrivateFieldSet(_age, this, age); // Using private field
  }
  return _createClass(Person, [{
    key: "introduce",
    value: function introduce() {
      console.log("Hello, my name is ".concat(this.name, " and I am ").concat(_assertClassBrand(_Person_brand, this, _getAge).call(this), " years old."));
    }
  }]);
}();
function _getAge() {
  return _classPrivateFieldGet(_age, this);
}
var arrowFunctionExample = function arrowFunctionExample(a) {
  var b = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 5;
  return a + b;
}; // Arrow function with default parameter

var destructuringExample = function destructuringExample(_ref) {
  var x = _ref.x,
    y = _ref.y;
  return "X: ".concat(x, ", Y: ").concat(y);
};
var restParametersExample = function restParametersExample() {
  for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
    args[_key] = arguments[_key];
  }
  return args.reduce(function (acc, curr) {
    return acc + curr;
  }, 0);
};
var optionalChainingExample = function optionalChainingExample(obj) {
  var _obj$property$subProp, _obj$property;
  return (_obj$property$subProp = obj === null || obj === void 0 || (_obj$property = obj.property) === null || _obj$property === void 0 ? void 0 : _obj$property.subProperty) !== null && _obj$property$subProp !== void 0 ? _obj$property$subProp : 'Default Value'; // Optional chaining
};
var replaceAllExample = function replaceAllExample() {
  var originalString = "Hello World! World is beautiful.";
  console.log("Original: ".concat(originalString));
  console.log("Modified: ".concat(originalString.replaceAll("World", "Universe")));
};
var numericSeparatorExample = function numericSeparatorExample() {
  var largeNumber = 1000000;
  console.log("Formatted Number (underscores): ".concat(largeNumber));
};
module.exports = {
  Person: Person,
  arrowFunctionExample: arrowFunctionExample,
  destructuringExample: destructuringExample,
  restParametersExample: restParametersExample,
  optionalChainingExample: optionalChainingExample,
  replaceAllExample: replaceAllExample,
  numericSeparatorExample: numericSeparatorExample
};