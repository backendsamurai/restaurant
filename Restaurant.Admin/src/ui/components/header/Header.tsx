import { useAppSelector } from '@/ui/hooks';
import { links } from '@ui/routes';
import { FC, useMemo } from 'react';
import { NavLink } from 'react-router';
import styles from './Header.module.scss';

export const Header: FC = () => {
	const { user } = useAppSelector((state) => state.auth);

	const navigationLinks = useMemo(() => {
		return links.filter((l) => {
			if (l.protected && l.roles == '*') return l;

			if (l.protected && l.roles.includes(user?.employeeRole ?? '')) return l;

			return null;
		});
	}, [user, links]);

	const initialsOfUserName = useMemo(() => {
		const parts = user?.userName.split(' ');
		let initials: string | undefined = '';

		if (parts && parts.length === 2) {
			const firstNameFirstLetter = parts[0].at(0);
			const lastNameFirstLetter = parts[1].at(0);

			if (firstNameFirstLetter && lastNameFirstLetter)
				initials = firstNameFirstLetter + lastNameFirstLetter;
		}

		return initials;
	}, [user]);

	return (
		<nav className={styles.header}>
			<section className={styles.logo}>
				<NavLink
					to={'/app'}
					className={({ isActive }) => (isActive ? styles.active : '')}
				>
					Restaurant.Admin
				</NavLink>
			</section>

			<ul className={styles.links}>
				{navigationLinks.map((l, i) => (
					<li key={i}>
						<NavLink
							className={({ isActive }) => (isActive ? styles.active : '')}
							to={l.path}
						>
							{l.title}
						</NavLink>
					</li>
				))}
			</ul>

			<section className={styles.profile}>
				<h3>{initialsOfUserName}</h3>
			</section>
		</nav>
	);
};
