import { IAuthenticationEmployeeResponse } from '_api/models';
import { FC, ReactNode, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';

interface ProtectedRouteProps {
	children: ReactNode;
	user: IAuthenticationEmployeeResponse | null;
	authenticated?: boolean;
	roles?: string[];
}

export const ProtectedRoute: FC<ProtectedRouteProps> = ({
	children,
	roles,
	authenticated,
	user,
}) => {
	const navigate = useNavigate();
	const [allowed, setAllowed] = useState<boolean>();

	useEffect(() => {
		if (authenticated === false) {
			setAllowed(false);
			navigate('/login');
			return;
		}

		if (authenticated && user) {
			if (roles && roles.length > 0 && !roles.includes(user.employeeRole)) {
				setAllowed(false);
				navigate('/forbidden');
				return;
			}
		}

		setAllowed(true);
	}, [authenticated]);

	return allowed ? children : null;
};
