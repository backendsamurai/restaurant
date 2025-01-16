import { httpClient } from '@/config';
import { IUser } from '@app/models/user';
import axios from 'axios';
import { FC, FormEvent, useState } from 'react';
import { useNavigate } from 'react-router';
import { toast, ToastContainer } from 'react-toastify';
import styles from './LoginPage.module.scss';

export const LoginPage: FC = () => {
	const [email, setEmail] = useState<string>('');
	const [password, setPassword] = useState<string>('');
	const [status, setStatus] = useState<string>('');

	const navigate = useNavigate();

	const handleForm = async (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		try {
			setStatus('pending');
			const response = await httpClient.post('/employees/authentication', {
				email,
				password,
			});

			if (response.status == 200) {
				setStatus('fulfilled');
				saveUserToLocalStorage(response.data.data as IUser);
				navigate('/');
			}
		} catch (e) {
			setStatus('rejected');
			let message = 'Server unavailable! Try again later';

			if (axios.isAxiosError(e) && e.response?.data) {
				message = e.response.data.error.message;
			}

			toast(message, {
				hideProgressBar: true,
				theme: 'dark',
				type: 'error',
				position: 'top-center',
			});
		}
	};

	const saveUserToLocalStorage = (user: IUser) => {
		localStorage.setItem('user', JSON.stringify(user));
	};

	return (
		<div className={styles.container}>
			<section className={styles.wrapper}>
				<section className={styles['login__section']}>
					<section>
						<h1>Log In</h1>
						<span>Please, Enter your details to login</span>
					</section>
					<form onSubmit={handleForm}>
						<section className={styles['form__control']}>
							<label htmlFor='email_address_input'>Your Email</label>
							<input
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								type='email'
								id='email_address_input'
								placeholder='Enter your email'
								autoComplete='email'
								required
							/>
						</section>
						<section className={styles['form__control']}>
							<label htmlFor='password_input'>Your Password</label>
							<input
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								type='password'
								id='password_input'
								placeholder='Enter your password'
								autoComplete='current-password'
								required
							/>
						</section>

						<section className={styles['form__control']}>
							<button type='submit' disabled={status === 'pending'}>
								Login
							</button>
						</section>
						<ToastContainer />
					</form>
				</section>
			</section>
		</div>
	);
};
