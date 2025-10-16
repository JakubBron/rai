# Node.js Language Features Demonstration

This project showcases various JavaScript language features through examples and tests. It includes features from both older and newer standards, demonstrating how they can be utilized in a Node.js environment.

## Features Demonstrated

- Classes
- Arrow Function Syntax
- Block-Scoped Variables
- Default Function Parameters
- Destructuring Assignments
- String Interpolation
- Rest Parameters
- `this` Behavior in Nested Functions
- Optional Chaining
- `replaceAll` for Strings
- Numeric Separators
- Private Accessors and Methods

## Project Structure

```
node-language-features
├── src
│   ├── index.js          # Entry point for the application
│   ├── features.js       # Contains functions demonstrating language features
│   ├── utils.js          # Utility functions for string manipulation and numeric separators
│   └── examples
│       └── run.js        # Runs examples of the features
├── test
│   └── features.test.js   # Tests for the functions in features.js
├── package.json           # Project metadata and dependencies
├── .gitignore             # Files and directories to ignore by Git
└── README.md              # Documentation for the project
```

## Getting Started

To run the examples and tests in this project, follow these steps:

1. Clone the repository:
   ```
   git clone <repository-url>
   cd node-language-features
   ```

2. Install the dependencies:
   ```
   npm install
   ```

3. Run the examples:
   ```
   node src/examples/run.js
   ```

4. Run the tests:
   ```
   npm test
   ```

## Compatibility Notes

- Ensure you are using Node.js version 14.x or higher to utilize features like optional chaining and `replaceAll`.
- Some features may not be compatible with older browsers, particularly those that do not support ES6 or later.

## License

This project is licensed under the MIT License.