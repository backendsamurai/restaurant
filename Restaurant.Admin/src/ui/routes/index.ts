import pages from '@ui/pages';

// using for react-router lib
export const publicRoutes = [{ path: '/login', page: pages.LoginPage }];

export const protectedRoutes = [
	{ path: '/app', page: pages.HomePage, role: '*' },
];

// using for navigation bar
export const links = [{ to: '/app', title: 'Home' }];
