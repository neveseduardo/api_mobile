import { useAdminAuth } from '@/contexts/AdminAuthenticationContext';
import { View, Text, TouchableOpacity } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import { ThemedText } from '@/components/ui/ThemedText';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { z } from 'zod';

const formSchema = z.object({
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
	password: z.string().min(6, 'A senha deve ter pelo menos 6 caracteres'),
});

type InnerFormData = z.infer<typeof formSchema>;

export default function LoginScreen() {
	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			email: 'administrador@administrador.com',
			password: 'Senh@123',
		},
	});

	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const { login } = useAdminAuth();

	const onSubmit = async (data: InnerFormData) => {
		try {
			setLoading(true);

			const { accessToken, user } = await login(data.email, data.password);

			if (accessToken && user) {
				router.replace('/(adminzone)' as Href);
			}
		} catch (error: any) {
			console.error('Erro de Authenticação', error);
			if (error?.response?.status === 401) {
				setError('Usuário ou senha inválidos. Tente novamente!');
			}
			setError('Erro! Não conseguimos conectar com o servidor.');
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="relative items-center justify-center flex-1 p-8 bg-gray">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					icon="lock-closed-outline"
					title="Área reservada"
					description="Este login serve somente para administradores utilizarem a plataforma."
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

					<View className="flex flex-row items-center justify-center w-full gap-2 text-center">
						<ThemedText className="leading-10 align-middle">Não consegue autenticar?</ThemedText>
						<TouchableOpacity
							className=""
							onPress={() => router.push('/(auth)/userlogin')}
						>

							<Text className="font-semibold text-blue-400">Login de usuário</Text>
						</TouchableOpacity>
					</View>
				</View>
			</View>
		</ThemedView>
	);
}