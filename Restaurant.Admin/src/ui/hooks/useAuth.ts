import { setUser } from '@/app/store/reducers/auth';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useAppSelector } from './useAppSelector';

export const useAuth = () => {
	const dispatch = useDispatch();
	const { user } = useAppSelector((state) => state.auth);

	useEffect(() => {
		if (!user) {
			const savedUser = JSON.parse(localStorage.getItem('user') ?? 'null');

			if (savedUser) {
				localStorage.removeItem('user');
				localStorage.setItem('user', JSON.stringify(savedUser));
				dispatch(setUser(savedUser));
			} else {
				dispatch(setUser(null));
			}
		}
	}, [user, dispatch]);

	return user;
};
