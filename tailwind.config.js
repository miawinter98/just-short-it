/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["Pages/**/*.cshtml", "Components/**/*.razor"],
    theme: {
        extend: {
            screens: {
                '3xl': "1792px",
                '4xl': "2048px",
                '5xl': "2304px",
                '6xl': "2560px",
                '8xl': "3072px"
            },
            container: {
                'max-width': {
                    '3xl': "1792px",
                    '4xl': "2048px",
                    '5xl': "2304px",
                    '6xl': "2560px",
                    '8xl': "3072px"
                }
            }
        },
        fontSize: {
            sm: '0.750rem',
            base: '1rem',
            xl: '1.333rem',
            '2xl': '1.777rem',
            '3xl': '2.369rem',
            '4xl': '3.158rem',
            '5xl': '4.210rem'
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