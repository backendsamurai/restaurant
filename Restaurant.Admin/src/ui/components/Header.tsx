import { links } from '@ui/routes';
import { FC } from 'react';
import { NavLink } from 'react-router';

export const Header: FC = () => (
	<nav>
		<ul>
			{links.map((l, i) => (
				<li key={i}>
					<NavLink to={l.to}>{l.title}</NavLink>
				</li>
			))}
		</ul>
	</nav>
);
