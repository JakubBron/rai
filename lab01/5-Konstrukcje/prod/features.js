class Person {
    #age; // Private field

    constructor(name, age) {
        this.name = name;
        this.#age = age; // Using private field
    }

    // Private method
    #getAge() {
        return this.#age;
    }

    introduce() {
        console.log(`Hello, my name is ${this.name} and I am ${this.#getAge()} years old.`);
    }
}

const arrowFunctionExample = (a, b = 5) => a + b; // Arrow function with default parameter

const destructuringExample = ({ x, y }) => {
    return `X: ${x}, Y: ${y}`;
};

const restParametersExample = (...args) => {
    return args.reduce((acc, curr) => acc + curr, 0);
};

const optionalChainingExample = (obj) => {
    return obj?.property?.subProperty ?? 'Default Value'; // Optional chaining
};

const replaceAllExample = () => {
    const originalString = "Hello World! World is beautiful.";
    console.log(`Original: ${originalString}`);
    console.log(`Modified: ${originalString.replaceAll("World", "Universe")}`);
};

const numericSeparatorExample = () => {
    const largeNumber = 1_000_000;
    console.log(`Formatted Number (underscores): ${largeNumber}`);
};

module.exports = {
    Person,
    arrowFunctionExample,
    destructuringExample,
    restParametersExample,
    optionalChainingExample,
    replaceAllExample,
    numericSeparatorExample,
};