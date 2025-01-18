import { store } from '@app/store';
import '@assets/styles/index.scss';
import { protectedRoutes, publicRoutes } from '@ui/routes';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router';
import { ProtectedRoute } from './ui/components/ProtectedRoute';

createRoot(document.getElementById('root')!).render(
	<StrictMode>
		<Provider store={store}>
			<BrowserRouter>
				<Routes>
					<Route path='/' element={<Navigate to='/app' />} />
					{publicRoutes.map((r, i) => (
						<Route key={i} path={r.path} element={<r.page />} />
					))}

					{protectedRoutes.map((r, i) => (
						<Route
							key={i}
							path={r.path}
							element={<ProtectedRoute page={r.page} role={r.role} />}
						/>
					))}
				</Routes>
			</BrowserRouter>
		</Provider>
	</StrictMode>
);
