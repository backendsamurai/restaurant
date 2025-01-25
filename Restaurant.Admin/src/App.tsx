import { useAppDispatch, useAppSelector } from '_hooks';
import { ProtectedRoute, routes } from '_routes';
import { initAction } from '_store/auth';
import { FC, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router';

const App: FC = () => {
	const { user, authenticated } = useAppSelector((state) => state.auth);
	const dispatch = useAppDispatch();

	useEffect(() => {
		if (!authenticated) {
			dispatch(initAction());
		}
	}, [authenticated, dispatch]);

	return (
		<BrowserRouter>
			<Routes>
				{routes.map((r, i) =>
					r.protected ? (
						<Route
							path={r.path}
							key={i}
							element={
								<ProtectedRoute
									authenticated={authenticated}
									user={user}
									roles={r.roles}
								>
									<r.page />
								</ProtectedRoute>
							}
						/>
					) : (
						<Route path={r.path} key={i} element={<r.page />} />
					)
				)}
			</Routes>
		</BrowserRouter>
	);
};

export default App;
