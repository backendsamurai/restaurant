import { useAuth } from '@ui/hooks';
import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router';

export const App: FC = () => {
	const user = useAuth();
	const navigate = useNavigate();

	useEffect(() => {
		if (user === null) {
			navigate('/login');
			return;
		}

		navigate('/app');
	}, [user]);

	return null;
};
