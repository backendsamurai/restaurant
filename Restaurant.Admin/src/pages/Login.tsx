import * as chakra from '@chakra-ui/react';
import { Field, PasswordInput } from '_components/ui';
import { useAppDispatch, useAppSelector, useToast } from '_hooks';
import { authenticate } from '_store/auth/api';
import { FC, useEffect } from 'react';
import { SubmitHandler, useForm } from 'react-hook-form';
import { LuLogIn } from 'react-icons/lu';
import { useNavigate } from 'react-router';
import { ToastContainer } from 'react-toastify';

interface ILoginForm {
	email: string;
	password: string;
}

export const Login: FC = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const { status, error } = useAppSelector((state) => state.auth);
	const showToast = useToast();

	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm<ILoginForm>({ mode: 'all' });

	const onSubmit: SubmitHandler<ILoginForm> = async (params) => {
		dispatch(authenticate(params));
	};

	useEffect(() => {
		showToast({
			status,
			successMessage: 'You successfully logged in!',
			errorMessage: error,
			closeAfterMs: 500,
			onClose: () => navigate('/'),
		});
	}, [status]);

	return (
		<chakra.Container maxW={'md'} h={'vh'} alignContent={'center'}>
			<chakra.Stack>
				<chakra.Heading fontSize={'4xl'} textAlign={'center'} mb={'10'}>
					Sign in
				</chakra.Heading>
				<form onSubmit={handleSubmit(onSubmit)}>
					<chakra.Box mb={'6'}>
						<Field invalid={!!errors.email} errorText={errors.email?.message}>
							<chakra.Input
								px={'3'}
								variant={'subtle'}
								fontSize={'md'}
								colorPalette={'teal'}
								type={'email'}
								disabled={status === 'pending'}
								autoComplete={'email'}
								placeholder={'Enter your email address'}
								{...register('email', {
									required: 'Email address is required !',
								})}
							/>
						</Field>
					</chakra.Box>

					<chakra.Box mb={'6'}>
						<Field
							invalid={!!errors.password}
							errorText={errors.password?.message}
						>
							<PasswordInput
								px={'3'}
								fontSize={'md'}
								variant={'subtle'}
								colorPalette={'teal'}
								type={'password'}
								maxLength={16}
								disabled={status === 'pending'}
								placeholder={'Enter your password'}
								autoComplete={'current-password'}
								{...register('password', {
									required: 'Password is required !',
								})}
							/>
						</Field>
					</chakra.Box>

					<chakra.Button
						type={'submit'}
						w={'full'}
						fontSize={'md'}
						colorPalette={'teal'}
						variant={'surface'}
						disabled={
							status === 'pending' || !!errors.email || !!errors.password
						}
						loading={status === 'pending'}
					>
						<LuLogIn />
						Log in
					</chakra.Button>
				</form>
			</chakra.Stack>
			<ToastContainer position='top-center' />
		</chakra.Container>
	);
};
