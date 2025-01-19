import { useAuth } from '@ui/hooks';
import { FC, ReactNode, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';

interface IProtectedRouteProps {
	page: ReactNode;
	roles: '*' | string[];
}

export const ProtectedRoute: FC<IProtectedRouteProps> = ({ page, roles }) => {
	const user = useAuth();
	const navigate = useNavigate();
	const [allowed, setAllowed] = useState<boolean>();

	useEffect(() => {
		if (user === null) {
			setAllowed(false);
			navigate('/login');
			return;
		}

		if (user && roles !== '*' && roles.length > 0) {
			if (!roles.includes(user?.employeeRole ?? '')) {
				setAllowed(false);
				navigate('/forbidden');
				return;
			}
		}

		setAllowed(true);
	}, [user, roles]);

	return allowed ? <>{page}</> : null;
};
