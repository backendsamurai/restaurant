import { useColorMode } from '_components/ui';
import { Status } from '_types';
import { useCallback } from 'react';
import { toast, ToastOptions } from 'react-toastify';

interface UseToastOptions {
	status: Status;
	successMessage: string;
	errorMessage?: string;
	closeAfterMs?: number;
	onClose?: () => void;
}

type UseToastReturn = (options: UseToastOptions) => void;

export const useToast = (): UseToastReturn => {
	const { colorMode } = useColorMode();

	const toastOptions: ToastOptions = {
		hideProgressBar: true,
		theme: colorMode,
		closeButton: false,
		closeOnClick: false,
		pauseOnHover: false,
		pauseOnFocusLoss: false,
	};

	const showToast = useCallback(
		({
			status,
			successMessage,
			errorMessage,
			closeAfterMs = 350,
			onClose,
		}: UseToastOptions) => {
			const toastType = status === 'fulfilled' ? 'success' : 'error';

			if (status === 'fulfilled') {
				toast(successMessage, {
					type: toastType,
					...toastOptions,
					autoClose: closeAfterMs,
					onClose: onClose,
				});
			}

			if (status === 'rejected') {
				toast(errorMessage, {
					type: toastType,
					autoClose: closeAfterMs,
					...toastOptions,
				});
			}
		},
		[]
	);

	return showToast;
};
