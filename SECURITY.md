# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability in this portfolio website, please report it responsibly by contacting me directly rather than opening a public issue.

**Please DO NOT create a public GitHub issue for security vulnerabilities.**

### How to Report

📧 **Email:** dondamitz@gmail.com

Please include the following information in your report:
- Description of the vulnerability
- Steps to reproduce the issue
- Potential impact
- Suggested fix (if you have one)

### What to Expect

- I will acknowledge receipt of your report within 48 hours
- I will investigate the issue and provide an estimated timeline for a fix
- I will keep you informed of progress toward resolution
- Once the issue is resolved, you will receive credit for the discovery (if desired)

## Supported Versions

This is a personal portfolio website currently deployed in production. Security updates are applied as needed.

| Version | Supported          |
| ------- | ------------------ |
| Current deployment | ✅ |

## Security Features

This portfolio website implements several security measures:

- **SQL Injection Prevention:** Parameterized queries using ADO.NET SqlParameter
- **XSS Protection:** Automatic output encoding via ASP.NET Core Razor Pages
- **CSRF Protection:** Anti-forgery tokens on all form submissions
- **Rate Limiting:** Maximum 5 contact form submissions per IP address per 15 minutes
- **Input Validation:** Server-side validation using Data Annotations
- **HTTPS:** SSL/TLS encryption for all data transmission
- **Secure Headers:** Security headers configured in production
- **Firewall:** Azure SQL Database firewall restricts access to Azure App Service only

## Scope

**In scope for security reports:**
- SQL injection vulnerabilities
- Cross-site scripting (XSS)
- Cross-site request forgery (CSRF)
- Authentication/authorization bypasses
- Rate limiting bypasses
- Data exposure issues
- Server-side request forgery (SSRF)

**Out of scope:**
- Social engineering attacks
- Physical security
- Denial of service attacks (DDoS)
- Reports from automated tools without proof of exploitability
- Issues in third-party libraries (report to the library maintainers)

## Thank You

Thank you for helping keep this portfolio website secure! Responsible disclosure is appreciated and helps maintain a secure web for everyone.

---

**Project:** Don Damitz Portfolio Website  
**Repository:** https://github.com/damit7/DonDamitzWebsite  
**Live Site:** https://dondamitz.com