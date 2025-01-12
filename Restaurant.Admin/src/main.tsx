import '@assets/styles/index.css';
import { routes } from '@ui/routes';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router';

createRoot(document.getElementById('root')!).render(
	<StrictMode>
		<BrowserRouter>
			<Routes>
				{routes.map((r, i) => (
					<Route key={i} path={r.path} element={<r.page />} />
				))}
			</Routes>
		</BrowserRouter>
	</StrictMode>
);
