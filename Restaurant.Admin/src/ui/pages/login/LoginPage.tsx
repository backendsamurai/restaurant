import { FC, FormEvent, useState } from 'react';
import './LoginPage.css';

export const LoginPage: FC = () => {
	const [email, setEmail] = useState<string>('');
	const [password, setPassword] = useState<string>('');

	const handleForm = (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();
	};

	return (
		<div className='container'>
			<section className='login-form__section_wrapper'>
				<section className='login-form__section'>
					<section className='login-form__info'>
						<h1 className='login-form__heading'>Log In</h1>
						<span className='login-form__subheading'>
							Please, Enter your details to login
						</span>
					</section>
					<form action='#' className='login-form__form' onSubmit={handleForm}>
						<section className='login-form__control'>
							<label
								className='login-form__control_label'
								htmlFor='email_address_input'
							>
								Your Email
							</label>
							<input
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								type='email'
								className='login-form__control_input'
								id='email_address_input'
								placeholder='Enter your email'
								autoComplete='email'
								required
							/>
						</section>
						<section className='login-form__control'>
							<label
								className='login-form__control_label'
								htmlFor='password_input'
							>
								Your Password
							</label>
							<input
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								className='login-form__control_input'
								type='password'
								id='password_input'
								placeholder='Enter your password'
								autoComplete='current-password'
								required
							/>
						</section>

						<section className='login-form__control'>
							<button type='submit' className='login-form__control_submit'>
								Login
							</button>
						</section>
					</form>
				</section>
			</section>
		</div>
	);
};
