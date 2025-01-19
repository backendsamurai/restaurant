import { store } from '@app/store';
import '@assets/styles/index.scss';
import { ProtectedRoute } from '@ui/components/ProtectedRoute';
import { routes } from '@ui/routes';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter, Route, Routes } from 'react-router';
import { App } from './App';

createRoot(document.getElementById('root')!).render(
	<StrictMode>
		<Provider store={store}>
			<BrowserRouter>
				<Routes>
					<Route path='/' element={<App />} />
					{routes.map((r, i) => (
						<Route
							key={i}
							path={r.path}
							element={
								r.protected ? (
									<ProtectedRoute page={<r.page />} roles={r.roles} />
								) : (
									<r.page />
								)
							}
						/>
					))}
				</Routes>
			</BrowserRouter>
		</Provider>
	</StrictMode>
);
