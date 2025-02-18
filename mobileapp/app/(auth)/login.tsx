import { useState } from 'react';
import { View, Text } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import { ThemedText } from '@/components/ThemedText';
import { useAuth } from '@/contexts/AuthContext';
import AuthHeader from '@/components/auth/AuthHeader';
import Ionicons from '@expo/vector-icons/Ionicons';
import { z } from 'zod';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';


const loginSchema = z.object({
	email: z.string().email('Email inválido').nonempty('Email é obrigatório'),
	password: z.string().min(6, 'A senha deve ter pelo menos 6 caracteres').nonempty('Senha é obrigatória'),
});
type LoginFormData = z.infer<typeof loginSchema>;

export default function LoginScreen() {
	const { control, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			email: 'email@email.com',
			password: 'Senh@123',
		},
	});

	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const { login } = useAuth();

	const onSubmit = async (data: LoginFormData) => {
		try {
			setLoading(true);

			const { accessToken, user } = await login(data.email, data.password);

			if (accessToken && user) {
				router.replace('/(tabs)' as Href);
			}
		} catch (error: any) {
			if (error.response.status === 401) {
				console.error('Não autorizado');
				setError('Usuário ou senha inválidos. Tente novamente!');
			}
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					title="Authenticação"
					description="Insira os dados solicitados abaixo para autenticar no aplicativo."
				/>

				{error && (
					<View className="flex flex-row items-center w-full gap-2 p-2 bg-red-200 rounded">
						<Ionicons name="information-circle-outline" size={20} className="text-red-500" />
						<Text className="text-red-500">{error}</Text>
					</View>
				)}

				<View className="flex flex-col w-full gap-4">
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
						name="password"
						render={({ field: { onChange, onBlur, value } }) => (
							<PasswordInput
								placeholder="Senha"
								returnKeyType="done"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.password}
								errorMessage={errors.password?.message}
							/>
						)}
					/>

					<Button
						onPress={handleSubmit(onSubmit)}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<Text className="text-white">ENTRAR</Text>
					</Button>

					<Button
						onPress={() => router.push('/(auth)/register')}
						className="w-full"
						disabled={loading}
					>
						<ThemedText>REGISTRAR</ThemedText>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}
