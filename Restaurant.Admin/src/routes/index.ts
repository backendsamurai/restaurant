import {
	RemixiconComponentType,
	RiBowlLine,
	RiGroupLine,
	RiHome5Line,
	RiMapPin2Line,
	RiMoneyDollarCircleLine,
	RiServiceBellLine,
} from '@remixicon/react';
import pages from '_pages';
import { Forbidden, NotFound } from '_pages/common';
import { FC } from 'react';

interface IRoute {
	path: string;
	page: FC;
	protected: boolean;
	roles?: string[];
}

interface INavLink extends Omit<IRoute, 'page'> {
	title: string;
	icon?: RemixiconComponentType;
	hidden: boolean;
}

const links = [
	{
		title: 'Login',
		path: '/login',
		page: pages.Login,
		hidden: true,
		protected: false,
	},
	{
		title: 'Home',
		icon: RiHome5Line,
		path: '/',
		page: pages.Home,
		hidden: false,
		protected: true,
	},
	{
		title: 'Employees',
		icon: RiGroupLine,
		path: '/employees',
		page: pages.Home,
		roles: ['manager'],
		hidden: false,
		protected: true,
	},
	{
		title: 'Products',
		icon: RiBowlLine,
		path: '/products',
		page: pages.Home,
		hidden: false,
		protected: true,
	},
	{
		title: 'Orders',
		icon: RiServiceBellLine,
		path: '/orders',
		page: pages.Home,
		hidden: false,
		protected: true,
	},
	{
		title: 'Desks',
		icon: RiMapPin2Line,
		path: '/desks',
		page: pages.Desks,
		hidden: false,
		protected: true,
	},
	{
		title: 'Payments',
		icon: RiMoneyDollarCircleLine,
		path: '/payments',
		page: pages.Home,
		roles: ['manager'],
		hidden: false,
		protected: true,
	},
	{
		title: 'Profile',
		path: '/profile',
		page: pages.Home,
		hidden: true,
		protected: true,
	},
	{
		title: '404 Not Found',
		path: '*',
		page: NotFound,
		roles: [],
		hidden: true,
		protected: false,
	},
	{
		title: 'Forbidden Access',
		path: '/forbidden',
		page: Forbidden,
		hidden: true,
		protected: false,
	},
];

export const routes: IRoute[] = links.map(
	(l): IRoute => ({
		page: l.page,
		path: l.path,
		protected: l.protected,
		roles: l.roles,
	})
);

export const navigationLinks: INavLink[] = links
	.filter((l) => !l.hidden)
	.map(
		(l): INavLink => ({
			title: l.title,
			path: l.path,
			hidden: l.hidden,
			protected: l.protected,
			icon: l.icon,
			roles: l.roles,
		})
	);

export * from './ProtectedRoute.tsx';
