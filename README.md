# MiniMate 🌤️

A modular digital companion with useful everyday tools, built with Blazor WebAssembly - designed to help kids learn and make smart daily decisions.

## What is MiniMate?
MiniMate is a modern web application that combines various practical modules in a unified, user-friendly interface. Each module is developed independently and can be extended separately. The goal is to help children learn to make informed decisions about their daily activities.

## Current Modules:
- 🌤️ **Weather** - Current weather data with location search
- 👕 **Clothing** - Smart clothing recommendations based on weather conditions
- 📅 **Calendar** - Calendar and event management
- 👤 **Profile** - User profile management
- 📍 **Location** - Location services and geolocation

## 🛠️ Technology Stack

**Frontend & Framework**
- .NET 10
- Blazor WebAssembly
- C# 12

**Architecture & Patterns**
- Clean Architecture (Domain, Application, Infrastructure, UI)
- Modular Monolith
- Shared Kernel Pattern

**Features**
- Localization (German/English)
- REST API Integration
- CSS Isolation

**External APIs**
- Open-Meteo API (Weather & Geocoding)
- Geolocation API

## 🏗️ Architecture

**Clean Architecture Layers:**
- **Domain** - Business logic and entities
- **Application** - Services, contracts, and use cases
- **Infrastructure** - External services and API integrations
- **UI** - Blazor components with code-behind pattern

**Module Structure:**
Each module is a self-contained project with its own layers, resources, and service registration

## ✨ Features:
- 📱 Responsive design for all devices
- 🔍 Smart location search with autocomplete
- 📍 GPS location support worldwide
- 🌍 Multi-language support (German/English)
- ⚡ Fast and lightweight
- 🎨 Modern UI with smooth animations
- 👶 Kid-friendly interface
- 👕 Smart clothing recommendations based on weather
- 📅 Event and calendar management
- 👤 Personalized user profiles