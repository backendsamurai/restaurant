import pages from '@ui/pages';

// using for react-router lib
export const routes = [
	{ path: '/', page: pages.HomePage },
	{ path: '/login', page: pages.LoginPage },
];

// using for navigation bar
export const links = [
	{ to: '/', title: 'Home' },
	{ to: '/login', title: 'Login' },
];
