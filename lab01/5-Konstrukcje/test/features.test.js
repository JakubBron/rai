const { expect } = require('chai');
const path = require('path');
const features = require("../prod/features");

describe('JS Features', function() {
    let logs = [];
    let originalConsoleLog;

    beforeEach(function() {
        logs = [];
        originalConsoleLog = console.log;
        console.log = (...args) => {
            logs.push(args.join(' '));
        };
    });

    afterEach(function() {
        console.log = originalConsoleLog;
    });

    describe('Person', function() {
        it('should set name and keep age private; introduce logs correct message', function() {
            const p = new features.Person('Alice', 30);
            expect(p.name).to.equal('Alice');
            expect(p.age).to.equal(undefined);
            expect(p['#age']).to.equal(undefined);

            p.introduce();
            expect(logs.length).to.be.greaterThan(0);
            expect(logs[0]).to.include('Hello, my name is Alice and I am 30 years old.');
        });
    });

    describe('arrowFunctionExample', function() {
        it('should add with default parameter when second arg missing', function() {
            expect(features.arrowFunctionExample(7)).to.equal(12); // 7 + default 5
        });

        it('should add both provided arguments', function() {
            expect(features.arrowFunctionExample(2, 3)).to.equal(5);
        });
    });

    describe('destructuringExample', function() {
        it('should format x and y values', function() {
            expect(features.destructuringExample({ x: 10, y: 20 })).to.equal('X: 10, Y: 20');
        });

        it('works with zero/falsey numbers', function() {
            expect(features.destructuringExample({ x: 0, y: false })).to.equal('X: 0, Y: false');
        });
    });

    describe('restParametersExample', function() {
        it('should sum multiple arguments', function() {
            expect(features.restParametersExample(1, 2, 3, 4)).to.equal(10);
        });

        it('should return 0 when no arguments provided', function() {
            expect(features.restParametersExample()).to.equal(0);
        });
    });

    describe('optionalChainingExample', function() {
        it('should return nested subProperty when present', function() {
            const obj = { property: { subProperty: 'found' } };
            expect(features.optionalChainingExample(obj)).to.equal('found');
        });

        it('should return default value when path is missing', function() {
            expect(features.optionalChainingExample({})).to.equal('Default Value');
            expect(features.optionalChainingExample(null)).to.equal('Default Value');
            expect(features.optionalChainingExample(undefined)).to.equal('Default Value');
        });
    });

    describe('replaceAllExample', function() {
        it('should log original and modified strings with replacements', function() {
            features.replaceAllExample();
            // Expect at least two log lines: Original and Modified
            expect(logs.length).to.be.at.least(2);
            const joined = logs.join(' ');
            expect(joined).to.include('Original:');
            expect(joined).to.include('Modified:');
            expect(joined).to.include('Universe'); // replacement happened
            expect(joined).to.include('Hello'); // original content present
        });
    });

    describe('numericSeparatorExample', function() {
        it('should log the numeric value (underscores are numeric literal only)', function() {
            features.numericSeparatorExample();
            expect(logs.length).to.be.greaterThan(0);
            const joined = logs.join(' ');
            expect(joined).to.include('1000000');
            expect(joined).to.include('Formatted Number');
        });
    });
});