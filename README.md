Skinet marketplace


Skinet marketplace is a full-stack e-commerce web application built with ASP.NET Core Web API and Angular.
It provides a modern online shopping experience including product browsing, shopping cart, secure checkout, authentication, and Stripe payments.

🌐 Live Demo:
👉 https://skineproject.azurewebsites.net/

✨ Features

🛒 Product Catalog
Browse products with filtering, sorting, and pagination
🧺 Shopping Cart
Add, update, and remove items
👤 User Authentication
Register & login using JWT authentication
💳 Stripe Payment Integration
Secure checkout with Stripe Payment Intents
📦 Order Management
View order history and order details
📱 Responsive UI
Mobile-friendly Angular frontend
🔐 Secure Backend
Clean architecture with separation of concerns

<img width="1861" height="860" alt="skienet 33" src="https://github.com/user-attachments/assets/96164729-982a-4930-93be-605ce0268eed" />
<img width="1837" height="871" alt="skinet 66" src="https://github.com/user-attachments/assets/a88b777a-e087-473b-87ea-ea7438359651" />
<img width="1677" height="721" alt="skinet 44" src="https://github.com/user-attachments/assets/71146d29-2396-440a-8d6a-6814297e605b" />
<img width="1712" height="755" alt="skinets 55" src="https://github.com/user-attachments/assets/ebbf03c9-a2ab-40dc-a15b-3209b935523e" />

🧱 Tech Stack

Frontend
Angular 18
TypeScript
RxJS
Bootstrap / CSS

Backend
ASP.NET Core 8 Web API
Entity Framework Core
SQL Server
JWT Authentication

Payments
Stripe API
Stripe Webhooks

Deployment
Azure App Service

Getting Started (Local Setup)
✅ Prerequisites

.NET SDK 8+
Node.js 18+
Angular CLI 18
Docker
Redis
Stripe account (test keys)

Backend Setup
git clone https://github.com/LiuSuny/Skinet-Ecommerce.git
cd Skinet-Ecommerce/API
dotnet restore
dotnet ef database update
dotnet run

API will run at:
https://localhost:5001

Frontend Setup
cd client
npm install
ng serve

Frontend runs at:
http://localhost:4200

Configuration
Backend – appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=[databasename];Trusted_Connection=True;TrustServerCertificate=True"
  },
  "StripeSettings": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_...",
    "WebhookSecret": "whsec_..."
  }
}


Frontend – environment.ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api/',
  stripePublicKey: 'pk_test_...'
};

🔔 Stripe Webhooks
Endpoint example:
/api/webhooks/stripe


Events listened to:
payment_intent.succeeded
payment_intent.payment_failed
Use ngrok for local webhook testing.

🧪 Testing

Stripe test card:
4242 4242 4242 4242
Any future expiry date
Any CVC

☁️ Deployment
The application is deployed using Azure App Service.

🌐 Production URL:
https://skineproject.azurewebsites.net/

🤝 Contributing
Contributions are welcome!
Fork the repository
Create a new feature branch
Commit your changes
Open a Pull Request

📄 License
This project is licensed under the MIT License.

🙏 Acknowledgements
Stripe for payment infrastructure
Angular & ASP.NET Core communities
Open-source contributors and documentation

