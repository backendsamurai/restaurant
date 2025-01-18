import { IAuthenticatedPageProps } from '@/ui/components/ProtectedRoute';
import layouts from '@ui/layouts';
import { FC } from 'react';

interface IHomePageProps extends IAuthenticatedPageProps {}

export const HomePage: FC<IHomePageProps> = ({ user, role }) => (
	<layouts.MainLayout>
		<h1>Home Page</h1>
		<h2>
			<i>{user?.userName}</i>
			<br />
			<u>{user?.userEmail}</u>
		</h2>
		<span>Role: {role}</span>
	</layouts.MainLayout>
);
