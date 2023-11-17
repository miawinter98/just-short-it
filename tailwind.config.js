/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["Pages/**/*.cshtml", "Components/**/*.razor"],
    theme: {
        extend: {
        }
    },
    plugins: [require("daisyui")],
    daisyui: {
        logs: false,
        themes: [
            {
                dark: {
                    "primary": "#4a3285",
                    "primary-content": "#26d962",
                    "secondary-content": "#1fad4e",
                    
                    "base-100": "#141414",
                    "base-content": "#fff",
                    
                    "--rounded-btn": ".125rem",

                    "fontFamily": 'BlinkMacSystemFont, -apple-system, "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", "Helvetica", "Arial", sans-serif',
                    "fontSize": "1.1rem",
                    "fontWeight": 600
                }
            }
        ]
    }
}