import { useAppSelector } from '@/ui/hooks';
import { RiSettings2Line } from '@remixicon/react';
import { links } from '@ui/routes';
import { FC, useMemo } from 'react';
import { NavLink } from 'react-router';
import styles from './Header.module.scss';

export const Header: FC = () => {
	const { user } = useAppSelector((state) => state.auth);

	const navigationLinks = useMemo(() => {
		return links.filter((link) => {
			if (link.protected && link.roles == '*') return link;

			if (link.protected && link.roles.includes(user?.employeeRole ?? ''))
				return link;

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
					<RiSettings2Line size={19} />
					Restaurant.Admin
				</NavLink>
			</section>

			<ul className={styles.links}>
				{navigationLinks.map((l, i) => (
					<li key={i}>
						<NavLink
							className={({ isActive }) => {
								let classNames = [];

								if (isActive) classNames.push(styles.active);

								if (l.icon) classNames.push(styles.linkWithIcon);

								return classNames.join(' ');
							}}
							to={l.path}
						>
							{l.icon && <l.icon size={19} />}
							{l.title}
						</NavLink>
					</li>
				))}
			</ul>

			<NavLink
				to='/profile'
				className={({ isActive }) =>
					isActive
						? `${styles.profileAvatar} ${styles.active}`
						: styles.profileAvatar
				}
			>
				{initialsOfUserName}
			</NavLink>
		</nav>
	);
};
