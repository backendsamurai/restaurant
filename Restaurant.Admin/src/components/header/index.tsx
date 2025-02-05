import { Center, Link, Tabs } from '@chakra-ui/react';
import { useAppSelector } from '_hooks';
import { navigationLinks } from '_routes';
import { FC } from 'react';
import { NavLink, useLocation, useNavigate } from 'react-router';

export const Header: FC = () => {
	const { user } = useAppSelector((state) => state.auth);
	const navigate = useNavigate();
	const location = useLocation();

	const links = navigationLinks.filter((link) => {
		if (user && link.protected && link.roles) {
			if (link.roles.length > 0 && link.roles.includes(user.employeeRole)) {
				return link;
			}
		}

		return link;
	});

	return (
		<Center>
			<Tabs.Root
				defaultValue={location.pathname}
				variant={'line'}
				my={'5'}
				colorPalette={'teal'}
				navigate={({ value }) => navigate(`${value}`)}
			>
				<Tabs.List gap='5' border={'none'}>
					{links.map((l, i) => (
						<Tabs.Trigger key={i} value={l.path} asChild>
							<Link unstyled asChild>
								<NavLink to={l.path}>
									{l.icon && <l.icon size={20} />} {l.title}
								</NavLink>
							</Link>
						</Tabs.Trigger>
					))}
				</Tabs.List>
			</Tabs.Root>
		</Center>
	);
};
