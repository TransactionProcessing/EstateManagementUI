/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Components/**/*.{razor,html}",
    "./Pages/**/*.{razor,html}",
    "./wwwroot/**/*.html"
  ],
  theme: {
    extend: {
      colors: {
        'admin-dark': '#343a40',
        'admin-darker': '#1f2125',
        'admin-sidebar': '#2c3034',
        'admin-primary': '#007bff',
        'admin-secondary': '#6c757d',
        'admin-success': '#28a745',
        'admin-info': '#17a2b8',
        'admin-warning': '#ffc107',
        'admin-danger': '#dc3545',
        'admin-light': '#f8f9fa',
        'admin-border': '#dee2e6',
      },
      boxShadow: {
        'admin': '0 0.125rem 0.25rem rgba(0,0,0,0.075)',
        'admin-lg': '0 0.5rem 1rem rgba(0,0,0,0.15)',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
}
