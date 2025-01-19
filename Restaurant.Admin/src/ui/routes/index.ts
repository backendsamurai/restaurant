import {
	RemixiconComponentType,
	RiBowlLine,
	RiGroupLine,
	RiMapPin2Line,
	RiMoneyDollarCircleLine,
	RiServiceBellLine,
} from '@remixicon/react';
import pages from '@ui/pages';
import commonPages from '@ui/pages/common';
import { FC } from 'react';

export interface IRoute {
	title: string;
	icon?: RemixiconComponentType | null;
	path: string;
	page: FC;
	roles: '*' | string[];
	hidden: boolean;
	protected: boolean;
}

export const routes: IRoute[] = [
	{
		title: 'Login',
		path: '/login',
		page: pages.LoginPage,
		roles: [],
		hidden: true,
		protected: false,
	},
	{
		title: 'Home',
		path: '/app',
		page: pages.HomePage,
		roles: '*',
		hidden: true,
		protected: true,
	},
	{
		title: 'Employees',
		icon: RiGroupLine,
		path: '/employees',
		page: pages.EmployeesPage,
		roles: ['manager'],
		hidden: false,
		protected: true,
	},
	{
		title: 'Products',
		icon: RiBowlLine,
		path: '/products',
		page: pages.ProductsPage,
		roles: '*',
		hidden: false,
		protected: true,
	},
	{
		title: 'Orders',
		icon: RiServiceBellLine,
		path: '/orders',
		page: pages.OrdersPage,
		roles: '*',
		hidden: false,
		protected: true,
	},
	{
		title: 'Desks',
		icon: RiMapPin2Line,
		path: '/desks',
		page: pages.DesksPage,
		roles: '*',
		hidden: false,
		protected: true,
	},
	{
		title: 'Payments',
		icon: RiMoneyDollarCircleLine,
		path: '/payments',
		page: pages.PaymentsPage,
		roles: ['manager'],
		hidden: false,
		protected: true,
	},
	{
		title: 'Profile',
		path: '/profile',
		page: pages.ProfilePage,
		roles: '*',
		hidden: true,
		protected: true,
	},
	{
		title: '404 Not Found',
		path: '*',
		page: commonPages.NotFoundPage,
		roles: [],
		hidden: true,
		protected: false,
	},
	{
		title: 'Forbidden Access',
		path: '/forbidden',
		page: commonPages.ForbiddenPage,
		roles: '*',
		hidden: true,
		protected: false,
	},
];

// using for navigation bar
export const links = routes
	.filter((r) => !r.hidden)
	.map(
		(r): Omit<IRoute, 'page'> => ({
			title: r.title,
			icon: r.icon,
			path: r.path,
			roles: r.roles,
			hidden: r.hidden,
			protected: r.protected,
		})
	);
