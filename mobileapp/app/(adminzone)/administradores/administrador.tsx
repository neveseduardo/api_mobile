import { useState } from 'react';
import { View, Text } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import Ionicons from '@expo/vector-icons/Ionicons';
import { isCPF } from '@/utils/helpers';
import { HttpClient } from '@/services/HttpClient';
import { AdminService } from '@/services/AdminService';
import { z } from 'zod';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';

const formSchema = z.object({
	name: z.string().min(1, 'Campo obrigatório'),
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
});
type InnerFormData = z.infer<typeof formSchema>;

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminService(client);

export default function UsuarioCreateScreen() {
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			name: (routeParams.name ?? '') as string,
			email: (routeParams.email ?? '') as string,
		},
	});

	const router = useRouter();
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const onSubmit = async (data: InnerFormData) => {
		try {
			setLoading(true);

			if (!routeParams.editable) {
				await service.addFromAdminAsync({ ...data, password: 'Senh@123' });
			}

			if (routeParams.editable) {
				await service.updateFromAdminAsync(Number(routeParams.id), { ...data, password: 'Senh@123' });
			}

			router.dismissAll();
			router.replace('/(adminzone)/administradores');
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
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray-500">
			<View className="flex flex-col w-full gap-5">
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

					<Button
						onPress={handleSubmit(onSubmit)}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<Text className="text-white">
							{routeParams.editable ? 'ATUALIZAR' : 'CADASTRAR'}
						</Text>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}