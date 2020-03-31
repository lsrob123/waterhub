/// <reference path="service.ts"/>
/// <reference path="home-screen.ts"/>
/// <reference path="businesses-screen.ts"/>
/// <reference path="contact-screen.ts"/>
/// <reference path="admin-screen.ts"/>

const service = new Service();
const homeScreen = new HomeScreen(service);
const adminScreen = new AdminScreen(service);
const businessesScreen = new BusinessesScreen(service);
const contactScreen = new ContactScreen(service);