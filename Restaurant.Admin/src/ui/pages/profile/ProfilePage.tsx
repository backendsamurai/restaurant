import { logout } from '@app/store/reducers/auth';
import layouts from '@ui/layouts';
import { FC } from 'react';
import { useDispatch } from 'react-redux';

export const ProfilePage: FC = () => {
	const dispatch = useDispatch();

	return (
		<layouts.MainLayout>
			<h1>Profile Page</h1>
			<button type='button' onClick={() => dispatch(logout())}>
				Logout
			</button>
		</layouts.MainLayout>
	);
};
