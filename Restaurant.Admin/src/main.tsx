import { Provider as ChakraProvider } from '_components/ui';
import { store } from '_store';
import { createRoot } from 'react-dom/client';
import { Provider as ReduxProvider } from 'react-redux';
import App from './App';

createRoot(document.getElementById('root')!).render(
	// <StrictMode>
	<ReduxProvider store={store}>
		<ChakraProvider>
			<App />
		</ChakraProvider>
	</ReduxProvider>
	// {/* </StrictMode> */}
);
