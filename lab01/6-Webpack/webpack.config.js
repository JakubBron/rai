const path = require("path");

module.exports = {
  mode: "development", // lub 'production'
  entry: "./src/index.js",
  output: {
    filename: "bundle.js",
    path: path.resolve(__dirname, "dist"),
    clean: true, // czyści stare pliki z dist/
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader",
        },
      },
    ],
  },
  devServer: {
    static: "./dist",   // serwuje pliki z folderu dist
    hot: true,          // 🔥 Hot Module Reloading
    open: true,         // automatycznie otwiera przeglądarkę
    port: 3000,         // http://localhost:3000
  },
  devtool: "source-map", // 🗺️ source mapping (o tym niżej)
};
