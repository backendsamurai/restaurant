import { IUser } from '@app/models/user';
import layouts from '@ui/layouts';
import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';

export const HomePage: FC = () => {
	const [user, setUser] = useState<IUser>();

	const navigate = useNavigate();

	useEffect(() => {
		const localUser = localStorage.getItem('user');

		if (localUser === null || localUser === '') {
			navigate('/login');
			return;
		}

		setUser(JSON.parse(localUser));
	}, []);

	return (
		<layouts.MainLayout>
			<h1>Home page</h1>
			<h3>Welcome, {user?.userName}</h3>
		</layouts.MainLayout>
	);
};
