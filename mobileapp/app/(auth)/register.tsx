import { useState } from 'react';
import { View, Text, TouchableOpacity } from 'react-native';
import { Href, useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import { ThemedText } from '@/components/ui/ThemedText';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import { useUserAuth } from '@/hooks/useUserAuth';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import Ionicons from '@expo/vector-icons/Ionicons';
import { isCPF } from '@/utils/helpers';
import Toast from 'react-native-root-toast';

const formSchema = z.object({
	name: z.string().min(1, 'Campo obrigatório'),
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
	password: z.string().min(6, 'A senha deve ter pelo menos 6 caracteres'),
	cpassword: z.string().min(6, 'A senha deve ter pelo menos 6 caracteres'),
	cpf: z.string()
		.min(11, 'O CPF deve ter 11 caracteres')
		.max(14, 'O CPF deve ter no máximo 14 caracteres')
		.refine(isCPF, { message: 'CPF inválido' }),
}).refine((data) => data.password === data.cpassword, {
	message: 'As senhas devem ser iguais',
	path: ['cpassword'],
});

type InnerFormData = z.infer<typeof formSchema>;

const cpfMask = [/\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];

export default function RegisterScreen() {
	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			name: 'Jhon Due',
			email: 'email@email.com',
			cpf: '737.425.159-93',
			password: 'Senh@123',
			cpassword: 'Senh@123',
		},
	});

	const router = useRouter();
	const params = useLocalSearchParams();
	const { addressId } = params;
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const { register, login } = useUserAuth();

	const onSubmit = async (data: InnerFormData) => {
		try {
			setLoading(true);

			await register(
				data.name,
				data.email,
				data.cpf,
				data.password,
				!isNaN(Number(addressId)) ? Number(addressId) : undefined,
			);

			const { accessToken, user } = await login(data.email, data.password);

			if (accessToken && user) {
				Toast.show('Usuário cadastrado com sucesso!', {
					duration: Toast.durations.SHORT,
					position: Toast.positions.BOTTOM,
					animation: true,
				});
				router.replace('/(userzone)' as Href);
			}
		} catch (error: any) {
			console.error('Erro de Authenticação', error);
			if (error?.response?.status === 401) {
				setError('Usuário ou senha inválidos. Tente novamente!');
				return;
			}

			if (error?.response?.status === 422) {
				setError(error?.response?.data?.message ?? 'O formulário contem dados inválidos');
				return;
			}

			setError('Não conseguimos conectar com o servidor.');
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray-500">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					icon="lock-closed-outline"
					title="Cadastre-se"
					description="Insira os dados solicitados abaixo para registrar-se no aplicativo."
				/>

				{!!error && (
					<View className="flex flex-row items-center w-full gap-2 p-2 bg-red-200 rounded">
						<Ionicons name="information-circle-outline" size={20} className="text-red-500" />
						<Text className="text-red-500">{error}</Text>
					</View>
				)}

				<View className="flex flex-col w-full gap-4">
					<Controller
						control={control}
						name="name"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								autoCapitalize="characters"
								returnKeyType="next"
								placeholder="name"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.name}
								errorMessage={errors.name?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="email"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								keyboardType="email-address"
								returnKeyType="next"
								placeholder="Email"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.email}
								errorMessage={errors.email?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="cpf"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="CPF"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.cpf}
								errorMessage={errors.cpf?.message}
								mask={cpfMask}
							/>
						)}
					/>

					<Controller
						control={control}
						name="password"
						render={({ field: { onChange, onBlur, value } }) => (
							<PasswordInput
								placeholder="Senha"
								returnKeyType="next"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.password}
								errorMessage={errors.password?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="cpassword"
						render={({ field: { onChange, onBlur, value } }) => (
							<PasswordInput
								placeholder="Confirmar Senha"
								returnKeyType="done"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.cpassword}
								errorMessage={errors.cpassword?.message}
							/>
						)}
					/>

					<Button
						onPress={handleSubmit(onSubmit)}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<Text className="text-white">FINALIZAR</Text>
					</Button>

					<View className="flex flex-row items-center justify-center w-full gap-2 text-center">
						<ThemedText className="leading-10 align-middle">Já possui uma conta?</ThemedText>
						<TouchableOpacity
							className=""
							onPress={() => router.push('/(auth)/userlogin')}
						>

							<Text className="font-semibold text-blue-400">Fazer login</Text>
						</TouchableOpacity>
					</View>
				</View>
			</View>
		</ThemedView>
	);
}