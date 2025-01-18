import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useAuth } from '../hooks';

interface IProtectedRouteProps {
	page: FC;
	roles: '*' | string[];
}

export const ProtectedRoute: FC<IProtectedRouteProps> = (props) => {
	const user = useAuth();
	const navigate = useNavigate();

	useEffect(() => {
		if (user === null) {
			navigate('/login');
			return;
		}
	}, [user]);

	return user !== null ? <props.page /> : null;
};
