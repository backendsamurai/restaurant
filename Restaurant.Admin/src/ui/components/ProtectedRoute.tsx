import { IUser } from '@app/models/user';
import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useAuth } from '../hooks';

export interface IAuthenticatedPageProps {
	user: IUser | null;
	role: string;
}

interface IProtectedRouteProps {
	page: FC<IAuthenticatedPageProps>;
	role: string;
}

export const ProtectedRoute: FC<IProtectedRouteProps> = (props) => {
	const navigate = useNavigate();
	const user = useAuth();

	useEffect(() => {
		if (user === null) {
			navigate('/login');
			return;
		}
	}, [user, navigate]);

	return <props.page role={props.role} user={user ?? null} />;
};
